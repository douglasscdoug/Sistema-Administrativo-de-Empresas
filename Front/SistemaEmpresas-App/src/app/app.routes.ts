import { Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout/layout.component';
import { authGuard } from './core/guards/auth.guard';
import { HomeComponent } from './features/dashboard/pages/home/home.component';

export const routes: Routes = [
    // rota pública
    {
        path: 'login',
        loadComponent: () =>
            import('./features/auth/pages/login/login.component')
                .then(m => m.LoginComponent)
    },

    // rotas protegidas
    {
        path: '',
        component: LayoutComponent,
        canActivate: [authGuard],
        children: [
            {
                path: '',
                redirectTo: 'dashboard',
                pathMatch: 'full'
            },
            {
                path: 'empresas',
                loadChildren: () =>
                    import('./features/empresa/empresa.routes')
                        .then(m => m.EMPRESA_ROUTES)
            },
            {
                path: 'usuarios',
                loadChildren: () =>
                    import('./features/usuario/usuario.routes')
                        .then(m => m.USUARIO_ROUTES)
            },
            {
                path: 'dashboard',
                loadChildren: () =>
                    import('./features/dashboard/dashboard.routes')
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