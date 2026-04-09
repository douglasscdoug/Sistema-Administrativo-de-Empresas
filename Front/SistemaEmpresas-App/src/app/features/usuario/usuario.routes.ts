import { Routes } from "@angular/router";

export const USUARIO_ROUTES: Routes = [
    {
        path: '',
        loadComponent: () =>
            import('./pages/usuario-list/usuario-list.component').then(m => m.UsuarioListComponent)
    },
    {
        path: 'novo',
        loadComponent: () =>
            import('./pages/usuario-detalhe/usuario-detalhe.component').then(m => m.UsuarioDetalheComponent)
    },
    {
        path: ':id',
        loadComponent: () =>
            import('./pages/usuario-detalhe/usuario-detalhe.component').then(m => m.UsuarioDetalheComponent)
    }
];