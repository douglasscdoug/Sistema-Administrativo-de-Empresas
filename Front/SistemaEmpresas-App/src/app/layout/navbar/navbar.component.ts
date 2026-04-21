import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router, RouterLinkActive, RouterModule } from "@angular/router";
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { AuthService } from '../../core/services/auth.service';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { HasRoleDirective } from '../../shared/directives/has-role.directive';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [
    RouterLinkActive,
    CommonModule,
    RouterModule,
    BsDropdownModule,
    CollapseModule,
    HasRoleDirective
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
})
export class NavbarComponent {
  isCollapsed = true;
  public authService = inject(AuthService);
  private router = inject(Router);

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  toggleMenu(): void {
    this.isCollapsed = !this.isCollapsed;
  }

  teste():any {
    console.log(this.authService.getRole());
  }
}
