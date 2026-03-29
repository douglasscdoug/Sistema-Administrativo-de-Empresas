import { Routes } from "@angular/router";
import { EmpresaListComponent } from "./pages/empresa-list/empresa-list.component";

export const EMPRESA_ROUTES: Routes = [
    {
        path: '',
        component: EmpresaListComponent
    }
];