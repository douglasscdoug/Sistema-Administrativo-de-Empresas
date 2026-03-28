import { Component } from '@angular/core';
import { Navbar } from "../navbar/navbar";
import { RouterModule, RouterOutlet } from "@angular/router";

@Component({
  selector: 'app-layout',
  imports: [Navbar, RouterOutlet, RouterModule],
  templateUrl: './layout.html',
  styleUrl: './layout.scss',
})
export class Layout {}
