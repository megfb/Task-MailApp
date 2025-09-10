import { Component } from "@angular/core";
import { UserModel } from "../../../core/models/user.model";
import { UserService } from "../../../core/services/user.service";
import { FormsModule } from "@angular/forms";


@Component({
  selector: 'app-user-update',
  imports: [FormsModule],
  templateUrl: './user-update.html'
})
export class UserUpdateComponent {
  user: UserModel = {
    userName: '',
    passwordHash: '',
    emailAddress: '',
    emailPassword: '',
    serverName: '',
    port: 993
  };

  constructor(private userService: UserService) { }

  update() {
    this.userService.updateUser(this.user).subscribe({
      next: () => {
        console.log('Kullanıcı güncellendi ');
      },
      error: (err) => {
        console.error('Kullanıcı güncelleme hatası', err);
      }
    });
  }
}
