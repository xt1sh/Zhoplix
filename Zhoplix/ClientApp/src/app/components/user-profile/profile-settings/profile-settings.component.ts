import { Component, OnInit, NgZone } from '@angular/core';
import { ProfileService } from 'src/app/services/profile/profile.service';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile-settings',
  templateUrl: './profile-settings.component.html',
  styleUrls: ['./profile-settings.component.scss']
})
export class ProfileSettingsComponent implements OnInit {

  private avatar: string;
  private email: string;
  private phone: string;
  private username: string;

  constructor(private readonly profileService: ProfileService,
              private readonly auth: AuthenticationService,
              private readonly router: Router,
              private readonly ngZone: NgZone) {}

  ngOnInit() {
  this.avatar = this.profileService.getProfileImage();
    this.profileService.getProfileSettings()
    .subscribe(value => {
      this.email = value.body.email;
      this.username = value.body.username;
      this.phone = value.body.phoneNumber;
    })
  }

  signOut() {
    this.auth.signOutOfAll();
    this.auth.deleteTokens();
    this.ngZone.run(() => this.router.navigate(['']));
  }

}
