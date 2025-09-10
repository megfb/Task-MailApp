export interface RegisterModel {

  UserName: string | null;
  PasswordHash: string | null;
  EmailAddress: string | null;
  EmailPassword?: string | null;
  ServerName?: string | null; 
  Port:number;
}
