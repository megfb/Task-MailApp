import { Component } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { LoginModel } from '../../../core/models/login.model';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastModel } from '../../../core/models/toast.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [FormsModule,CommonModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class Login {
  username = '';
  password = '';
  toasts: ToastModel[] = [];

  constructor(private authService: AuthService, private router: Router) { }

  //token sayfa açıldığında kontrol ediliyor. eğer token varsa main sayfasına yönlendiriyor.
  ngOnInit(): void {
    const token = localStorage.getItem('token');
    if (token) {
      this.router.navigate(['main']);
    }
  }

  //login işlemi yapılıyor.
  onSubmit(): void {
    //login model oluşturuldu
    const LoginQuery: LoginModel = {
      UserName: this.username,
      Password: this.password
    };

    //login sorgusuna login model gönderildi. başarılı-başarısız durumlarına göre aksiyon alındı.
    this.authService.login(LoginQuery).subscribe({
      next: (res) => {
        if (res.isSuccess && res.data) {
          localStorage.setItem('token', res.data);
          this.showToast('Giriş başarılı!', 'success');
          this.router.navigate(['main']);
        } else {
          this.showToast(res.errorMessage || 'Giriş başarısız', 'error');
        }
      },
      error: () => {
        this.showToast('Sunucuya ulaşılamıyor', 'error');
      }
    });
  }

  //register sayfasına yönlendir
  RouteRegister() {
    this.router.navigate(["register"]);
  }

  //hata durumunda toast oluştur.
  showToast(message: string, type: 'error' | 'success') {
    const toast: ToastModel = { message, type };
    this.toasts.push(toast);
    setTimeout(() => this.removeToast(toast), 4000);
  }

  //toastı kapat.
  removeToast(toast: ToastModel) {
    const index = this.toasts.indexOf(toast);
    if (index !== -1) {
      this.toasts.splice(index, 1);
    }
  }

}

