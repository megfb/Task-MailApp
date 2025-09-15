import { Component } from '@angular/core';
import { UserService } from '../../../core/services/user.service';

@Component({
  selector: 'app-user-delete',
  imports: [],
  templateUrl: './user-delete.html',
  styleUrl: './user-delete.css'
})
export class UserDelete {

  constructor(private userService: UserService) { }

  onDelete() {
    if (confirm('Hesabınızı silmek istediğinize emin misiniz?')) {
      this.userService.deleteUser().subscribe(() => {
        localStorage.removeItem('token');
        alert('Hesabınız silindi. Oturum kapatılıyor...');
        window.location.href = '';
      });
    }
  }
}
