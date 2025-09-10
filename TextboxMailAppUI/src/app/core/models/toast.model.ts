export interface ToastModel {
  message: string;           // Gösterilecek mesaj
  type: 'error' | 'success'; // Hata mı, başarı mı
}