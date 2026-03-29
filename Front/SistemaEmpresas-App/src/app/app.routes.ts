import { Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout/layout.component';
import { authGuard } from './core/guards/auth.guard';
import { HomeComponent } from './dashboard/pages/home/home.component';

export const routes: Routes = [
    // rota pública
    {
        path: 'login',
        loadComponent: () =>
            import('./auth/pages/login/login.component')
                .then(m => m.LoginComponent)
    },
  
    // rotas protegidas
    {
        path: '',
        component: LayoutComponent,
        canActivate: [authGuard],
        children: [
        {
            path: 'empresa',
            loadChildren: () =>
            import('./empresa/empresa.routes')
                .then(m => m.EMPRESA_ROUTES)
         },
        {
            path: 'dashboard',
            loadChildren: () =>
            import('./dashboard/dashboard.routes')
                .then(m => m.DASHBOARD_ROUTES)
        }
        ]
    },

    // fallback
    {
        path: '**',
        redirectTo: 'dashboard'
    }
];
