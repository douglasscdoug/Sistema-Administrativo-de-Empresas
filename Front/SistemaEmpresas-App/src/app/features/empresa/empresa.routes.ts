import { Routes } from "@angular/router";

export const EMPRESA_ROUTES: Routes = [
    {
        path: '',
        loadComponent: () =>
            import('./pages/empresa-list/empresa-list.component').then(m => m.EmpresaListComponent)
    },
    {
        path: 'nova',
        loadComponent: () =>
            import('./pages/empresa-detalhe/empresa-detalhe.component').then(m => m.EmpresaDetalheComponent)
    },
    {
        path: ':id',
        loadComponent: () =>
            import('./pages/empresa-detalhe/empresa-detalhe.component').then(m => m.EmpresaDetalheComponent)
    }

];