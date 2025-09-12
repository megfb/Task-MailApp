using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Application.Features.EmailMessages;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Persistence
{
  public class MailKitEmailReader : IEmailReader
  {
    private int PageSize = 100;

    public async Task<List<EmailMessagesDto>> GetEmailsByPageAsync(User user, uint? minExistingUid = null)
    {
      try
      {
        using var client = await ConnectAsync(user);
        var inbox = client.Inbox;
        await inbox.OpenAsync(FolderAccess.ReadOnly);

        if (inbox.Count == 0)
          return new List<EmailMessagesDto>();

        var allUids = await inbox.SearchAsync(SearchQuery.All);
        var filteredUids = minExistingUid.HasValue ? allUids.Where(uid => uid.Id < minExistingUid.Value) : allUids;

        var selectedUids = filteredUids.OrderByDescending(uid => uid.Id).Take(PageSize).ToList();

        if (!selectedUids.Any())
          return new List<EmailMessagesDto>();

        var summaries = await inbox.FetchAsync(selectedUids, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure);

        var result = await FetchMessagesAsync(inbox, summaries, user.Id);

        await client.DisconnectAsync(true);

        return result.OrderBy(x => x.Uid).ToList();
      }
      catch (Exception ex)
      {

        Console.WriteLine(ex);
        return new List<EmailMessagesDto>();
      }

    }

    public async Task<List<EmailMessagesDto>> GetEmailsAfterUidAsync(uint lastMaxUid, User user)
    {

      try
      {
        using var client = await ConnectAsync(user);
        var inbox = client.Inbox;
        await inbox.OpenAsync(FolderAccess.ReadOnly);

        if (inbox.Count == 0 || !inbox.UidNext.HasValue || inbox.UidNext.Value.Id <= lastMaxUid)
          return new List<EmailMessagesDto>();

        var startUidValue = lastMaxUid + 1;
        var endUidValue = inbox.UidNext.Value.Id - 1;

        if (startUidValue > endUidValue)
          return new List<EmailMessagesDto>();

        var uidRange = new List<UniqueId>();
        for (uint uid = startUidValue; uid <= endUidValue; uid++)
        {
          uidRange.Add(new UniqueId(uid));
        }

        var summaries = await inbox.FetchAsync(
            uidRange,
            MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure
        );

        var result = await FetchMessagesAsync(inbox, summaries, user.Id);

        await client.DisconnectAsync(true);

        return result.OrderByDescending(e => e.Uid).ToList();
      }
      catch (Exception ex)
      {

        Console.WriteLine(ex);
        return new List<EmailMessagesDto>();
      }


    }

    // refactor: mail sunucusuna bağlan
    private async Task<ImapClient> ConnectAsync(User user)
    {
      var client = new ImapClient();
      await client.ConnectAsync(user.ServerName, user.Port, SecureSocketOptions.SslOnConnect);
      await client.AuthenticateAsync(user.EmailAddress, user.EmailPassword);
      return client;
    }

    // refactor: summary listesinden gerçek mesajları çekip DTO’ya dönüştür
    private async Task<List<EmailMessagesDto>> FetchMessagesAsync(IMailFolder inbox, IList<IMessageSummary> summaries, string userId)
    {
      var result = new List<EmailMessagesDto>();

      foreach (var summary in summaries)
      {
        var message = await inbox.GetMessageAsync(summary.UniqueId);
        result.Add(MapToDto(summary, message, userId));
      }

      return result;
    }

    private EmailMessagesDto MapToDto(IMessageSummary summary, MimeMessage message, string userId)
    {

      var bodyHtml = message.HtmlBody ?? message.TextBody ?? string.Empty;

      // refactor: sadece düz metin snippet: TextBody varsa onu al, yoksa HtmlBody'den metin al
      var snippetText = message.TextBody ?? message.HtmlBody ?? string.Empty;

      // güvenli şekilde 100 karakterle kırp
      var snippet = snippetText.Length > 100 ? snippetText.Substring(0, 100) : snippetText;

      return new EmailMessagesDto
      {
        Uid = summary.UniqueId.Id,
        FromName = message.From.Mailboxes.FirstOrDefault()?.Name ?? "",
        FromAddress = message.From.Mailboxes.FirstOrDefault()?.Address ?? "",
        Subject = message.Subject,
        Snippet = snippet,
        Date = message.Date.DateTime,
        To = string.Join(",", message.To.Mailboxes.Select(x => x.Address)),
        Cc = string.Join(",", message.Cc.Mailboxes.Select(x => x.Address)),
        Body = bodyHtml,
        UserId = userId,
      };
    }


  }
}

