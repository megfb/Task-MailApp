import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserDelete } from '../../../../features/user/user-delete/user-delete';

@Component({
  selector: 'app-sidebar',
  imports: [UserDelete],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css'
})
export class Sidebar {

  constructor(private route:Router) {

  }

  RouteToProfile(){
    this.route.navigate(['main/updateuser']);
  }
}
