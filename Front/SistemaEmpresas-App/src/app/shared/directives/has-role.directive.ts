import { Directive, Input, OnDestroy, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { Subscription } from 'rxjs';

@Directive({
  selector: '[hasRole]',
  standalone: true
})
export class HasRoleDirective implements OnDestroy{

  private subscription?: Subscription;
  private currentRole?: string;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private authService: AuthService
  ) { }
  
  @Input() set hasRole(role: string) {
    this.currentRole = role;

    this.subscription?.unsubscribe();

    this.subscription = this.authService.user$.subscribe(user => {
      this.viewContainer.clear();

      if (user?.role === this.currentRole) {
        this.viewContainer.createEmbeddedView(this.templateRef);
      }
    });
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}
