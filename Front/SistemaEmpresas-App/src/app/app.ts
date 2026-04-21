import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from "@angular/router";
import { NgxSpinnerModule } from 'ngx-spinner';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NgxSpinnerModule],
  templateUrl: './app.html',
  styleUrl: './app.scss',
  standalone: true
})
export class App implements OnInit{
  protected readonly title = signal('SistemaEmpresas-App');
  private authService = inject(AuthService);

  ngOnInit(): void {
    this.authService.loadUser();
  }
}
