import { Component } from '@angular/core';
import { NavbarComponent } from "../navbar/navbar.component";
import { RouterModule, RouterOutlet } from "@angular/router";

@Component({
  selector: 'app-layout',
  imports: [NavbarComponent, RouterOutlet, RouterModule],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss',
})
export class LayoutComponent {}
