import { Routes } from "@angular/router";
import { HomeComponent } from "src/app/components/home/home.component";
import { AdminComponent } from "src/app/components/admin/admin/admin.component";
import { LoginComponent } from "src/app/components/authentication/login/login.component";
import { AuthGuardService } from "./auth-guard/auth-guard.service";
import { RoleGuardService } from "./role-guard/role-guard.service";
import { Roles } from "src/app/models/roles";
import { RegistrationComponent } from "src/app/components/authentication/registration/registration.component";
import { CreateComponent } from "src/app/components/admin/create/create/create.component";
import { TitleComponent } from '../../components/admin/create/title/title.component';
import { ConfirmEmailComponent } from "src/app/components/authentication/confirmEmail/confirmEmail.component";
import { PlayerComponent } from "src/app/components/player/player/player.component";
import { SeasonComponent } from '../../components/admin/create/season/season.component';
import { ProfileSettingsComponent } from "src/app/components/user-profile/profile-settings/profile-settings.component";

export const ROUTES: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full'},
  { path: 'login', component: LoginComponent},
  { path: 'signup/:id', component: RegistrationComponent},
  { path: 'signup', redirectTo: 'signup/1'},
  { path: 'account/confirmEmail', component: ConfirmEmailComponent },
  { path: 'player', component: PlayerComponent, pathMatch: 'full' },
  { path: 'profile', component: ProfileSettingsComponent, canActivate:[AuthGuardService], pathMatch: 'full'},
  {
    path: 'admin',
    component: AdminComponent,
    // canActivate: [AuthGuardService],
    data: {
      expectedRole: Roles.Admin
    },
    children: [{
        path: 'create',
        component: CreateComponent,
        children: [{
          path: 'title',
          component: TitleComponent
        },{
          path: 'season',
          component: SeasonComponent
        }]
      }
    ]
  },
  { path: '**', redirectTo: ''}
]
