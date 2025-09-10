import { Component } from '@angular/core';
import { RegisterModel } from '../../../core/models/register.model';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ToastModel } from '../../../core/models/toast.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  imports: [FormsModule, CommonModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {

  userName: string | null = null;
  password: string | null = null;
  emailAddress: string | null = null;
  emailPassword: string | null = null;
  serverName: string | null = null;
  port: number = 0;
  toasts: ToastModel[] = [];

  isLoading: boolean = false;

  constructor(private authService: AuthService, private router: Router) { }
  //token varsa sayfaya erişilmek istendiğinde main'e yönlendirir.
  ngOnInit(): void {
    const token = localStorage.getItem('token');
    if (token) {
      this.router.navigate(['main']);
    }
  }

  //resister işlemi çalışır.
  onSubmit(): void {
    this.isLoading = true;

    const registerQuery: RegisterModel = {
      UserName: this.userName,
      PasswordHash: this.password,
      EmailAddress: this.emailAddress,
      EmailPassword: this.emailPassword,
      ServerName: this.serverName,
      Port: this.port
    };

    //register model gönderilir. hata durumunda ona göre aksiyon alınır.
    this.authService.register(registerQuery).subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.isSuccess) {
          this.showToast('Kayıt başarılı!', 'success');
          this.router.navigate(['']);
        } else {
          this.showToast(res.errorMessage || 'Kayıt başarısız', 'error');
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.showToast('Kayıt başarısız', 'error');
      }
    });
  }

  //login sayfasına geri dön
  goToLogin(): void {
    this.router.navigate(['']);
  }

  //toast mesajı göster
  showToast(message: string, type: 'error' | 'success') {
    const toast: ToastModel = { message, type };
    this.toasts.push(toast);
    setTimeout(() => this.removeToast(toast), 4000);
  }

  //toast mesajı kapat
  removeToast(toast: ToastModel) {
    const index = this.toasts.indexOf(toast);
    if (index !== -1) this.toasts.splice(index, 1);
  }
}