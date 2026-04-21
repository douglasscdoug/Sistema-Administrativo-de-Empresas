import { Routes } from "@angular/router";
import { authGuard } from "../../core/guards/auth.guard";
import { adminGuard } from "../../core/guards/admin.guard";

export const USUARIO_ROUTES: Routes = [
    {
        path: '',
        loadComponent: () =>
            import('./pages/usuario-list/usuario-list.component').then(m => m.UsuarioListComponent),
        canActivate: [adminGuard]
    },
    {
        path: 'novo',
        loadComponent: () =>
            import('./pages/usuario-detalhe/usuario-detalhe.component')
                .then(m => m.UsuarioDetalheComponent),
        canActivate: [adminGuard]
    },
    {
        path: ':id',
        loadComponent: () =>
            import('./pages/usuario-detalhe/usuario-detalhe.component')
                .then(m => m.UsuarioDetalheComponent),
        canActivate: [adminGuard]
    }
];