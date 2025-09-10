import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmailService } from '../../../core/services/email.service';
import { Email } from '../../../core/models/email.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-emails',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './emails.html',
  styleUrls: ['./emails.css']
})
export class EmailsComponent implements OnInit {

  emails: Email[] = []; 
  pageNumber: number = 1;
  pageSize: number = 10;
  isLoading: boolean = false;
  searchTerm?: string; // arama input değeri
  sortBy: string | null = null;
  sortDesc: boolean = true; // Başlangıçta azalan sıralama  
  constructor(private emailService: EmailService) { }

  //sayfa açıldığında mailler çekilir
  ngOnInit(): void {
    this.loadEmails();
  }

  //mailler çekilir.
  loadEmails(): void {
    this.isLoading = true;
    this.emailService.GetMails(this.pageNumber, this.pageSize, this.searchTerm, this.sortBy, this.sortDesc)
      .subscribe({
        next: (res) => {
          if (res.isSuccess && res.data) {
            this.emails = res.data;
          }
          this.isLoading = false;
        },
        error: (err) => {
          console.error('Mail fetch error:', err);
          this.isLoading = false;
        }
      });
  }
  // başlık tıklanınca çağrılacak
  onSort(column: string): void {
    if (this.sortBy === column) {
      // aynı sütuna tekrar tıklayınca sıralama yönünü değiştir
      this.sortDesc = !this.sortDesc;
    } else {
      this.sortBy = column;
      this.sortDesc = true; // yeni sütun için varsayılan azalan
    }
    this.loadEmails();
  }
  // arama 
  onSearch(): void {
    this.sortBy = null;
    this.pageNumber = 1; // aramaya başlarken sayfayı 1'e sıfırla
    this.loadEmails();
  }
  //sonra ki sayfa
  nextPage(): void {
    this.pageNumber++;
    this.loadEmails();
  }

  //önce ki sayfa
  prevPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadEmails();
    }
  }


  selectedEmail: any = null;
  isModalOpen: boolean = false;

  
  //modal açılır. mail detayları gösterilir.
  showEmailDetail(id: string) {
    this.emailService.GetMail(id).subscribe({
      next: (res) => {
        this.selectedEmail = res.data; // backend'den gelen mail objesi
        this.isModalOpen = true;
      },
      error: (err) => console.error('Detay fetch error:', err)
    });
  }

  //modal kapatılır
  closeModal() {
    this.isModalOpen = false;
    this.selectedEmail = null;
  }

  //mailler yenilenir. ilk sayfadan tekrar sıralanır.
  refreshEmails() {
    this.isLoading = true;

    this.emailService.refreshMails().subscribe({
      next: () => {
        console.log('Mailler yenilendi');
        this.pageNumber = 1; // sayfayı başa al
        this.loadEmails();    // güncel mailleri tekrar yükle
      },
      error: (err) => {
        console.error('Refresh hatası:', err);
        this.isLoading = false;
      }
    });
  }
}
