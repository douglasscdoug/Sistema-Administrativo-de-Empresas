import { Routes } from '@angular/router';
import { Layout } from './layout/layout/layout';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
    {
        path: '',
        component: Layout,
        //canActivate: [authGuard],
        children: [
            {
                path: 'empresa',
                loadChildren: () =>
                    import('./empresa/empresa.routes').then(m => m.EMPRESA_ROUTES)
            }
        ]
    }
];
