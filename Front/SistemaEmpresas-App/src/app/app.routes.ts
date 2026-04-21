import { Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout/layout.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    // rota pública
    {
        path: 'login',
        loadComponent: () =>
            import('./features/auth/pages/login/login.component')
                .then(m => m.LoginComponent)
    },
    {
        path: 'acesso-negado',
        loadComponent: () =>
            import('./core/pages/acesso-negado/acesso-negado.component')
                .then(m => m.AcessoNegadoComponent)
    },
    {
        path: 'not-found',
        loadComponent: () =>
            import('./core/pages/not-found/not-found.component')
                .then(m => m.NotFoundComponent)
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
        redirectTo: 'not-found'
    }
];