export interface Email {
  id: string;
  uid: number;
  fromName: string;
  fromAddress: string;
  to: string;
  cc: string;
  subject: string;
  body: string;
  snippet: string;
  date: string;
  createdAt: string;
  updatedAt: string | null;
  userId: string;
}
