import { Component, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {

  constructor(private route:Router){

  }
  // Layout component'e event göndermek için
  @Output() logoutClicked = new EventEmitter<void>();

  // Logout butonuna tıklandığında çağrılacak
  onLogout(): void {
    this.logoutClicked.emit();
  }

  RouteToEmails(){
    this.route.navigate(['main']);
  }

}
