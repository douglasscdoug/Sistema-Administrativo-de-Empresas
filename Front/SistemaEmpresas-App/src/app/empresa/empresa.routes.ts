import { Routes } from "@angular/router";
import { EmpresaList } from "./pages/empresa-list/empresa-list";

export const EMPRESA_ROUTES: Routes = [
    {
        path: '',
        component: EmpresaList
    }
];