import { Component, OnInit } from '@angular/core';
import { ProfileService } from 'src/app/services/profile/profile.service';

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

  constructor(private readonly profileService: ProfileService) {}

  ngOnInit() {
  this.avatar = this.profileService.getProfileImage();
    this.profileService.getProfileSettings()
    .subscribe(value => {
      this.email = value.body.email;
      this.username = value.body.username;
      this.phone = value.body.phoneNumber;
    })
  }

}
