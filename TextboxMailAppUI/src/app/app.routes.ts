import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth-guard';

export const routes: Routes = [

    {
        path: "",
        loadComponent: () => import('./features/auth/login/login').then(x => x.Login)
    },
    {
        path: "register",
        loadComponent: () => import('./features/auth/register/register').then(x => x.Register)
    },
    {

        path: "main",
        canActivate:[AuthGuard],
        loadComponent: () => import('./core/layout/layout/layout').then(x => x.Layout),
        children: [
            {
                path: "",
                canActivate: [AuthGuard],
                loadComponent: () => import('./features/emails/emails/emails').then(x => x.EmailsComponent)
            },
            {
                path:"updateuser",
                canActivate:[AuthGuard],
                loadComponent:()=>import('./features/user/user-update/user-update').then(x => x.UserUpdateComponent)
            }
        ]
    },

];