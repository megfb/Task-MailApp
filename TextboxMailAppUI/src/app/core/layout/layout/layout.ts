import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { Navbar } from "../components/navbar/navbar";
import { Sidebar } from "../components/sidebar/sidebar";
import { Footer } from "../components/footer/footer";
import { UserDelete } from '../../../features/user/user-delete/user-delete';

@Component({
  selector: 'app-layout',
  imports: [RouterOutlet, Navbar, Sidebar, Footer,UserDelete],
  templateUrl: './layout.html',
  styleUrl: './layout.css'
})
export class Layout {
  constructor(private route: Router, private authService: AuthService) { }

  logout(): void {
    this.authService.logout(); // logout işlemi burada yapılacak
  }

  RouteToProfile() {
    this.route.navigate(['main/updateuser'])
  }
}
