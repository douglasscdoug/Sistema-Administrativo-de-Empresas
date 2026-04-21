import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';

@Directive({
  selector: '[hasRole]',
  standalone: true
})
export class HasRoleDirective {

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private authService: AuthService
  ) { }
  
  @Input() set hasRole(role: string | string[]) {
    const currentRole = this.authService.getRole();

    const roles = Array.isArray(role) ? role : [role];

    this.viewContainer.clear();

    if (currentRole && roles.includes(currentRole)) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    }

    console.log('ROLE ATUAL:', currentRole);
    console.log('ROLE ESPERADA:', roles);
  }
}
