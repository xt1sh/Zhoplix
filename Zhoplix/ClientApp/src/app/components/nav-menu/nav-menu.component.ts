import { Component, OnInit, NgZone } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { ProfileService } from 'src/app/services/profile/profile.service';

const routes: string[] = [];

@Component({
  selector: "app-nav-menu",
  templateUrl: "./nav-menu.component.html",
  styleUrls: ["./nav-menu.component.scss"]
})
export class NavMenuComponent implements OnInit {
  zhoplixLogoSrcLarge = "Logos/zhoplix_empty_167.png";
  zhoplixLogoSrcMedium = "Logos/zhoplix_empty_134.png";
  zhoplixLogoSrcSmall = "Logos/zhoplix_empty_108.png";
  isExpanded = false;
  isBlackBackground: boolean;
  toShowSignIn$: Observable<boolean>;
  toShowSignIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  toShowProfileMenu$: Observable<boolean>;
  toShowProfileMenu: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  avatar: string;

  constructor(private readonly router: Router,
              private readonly auth: AuthenticationService,
              private readonly profile: ProfileService,
              private readonly ngZone: NgZone) {
              }

  ngOnInit() {
    this.isBlackBackground = false;
    if (this.router.url.includes('/profile')) {
      this.isBlackBackground = true;
    }
    this.toShowSignIn$ = this.getToShowSignInValue;
    this.toShowProfileMenu$ = this.getToShowProfileMenu;
    this.avatar = this.profile.getProfileImage();
    const event = this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.toShowSignIn$ = this.getToShowSignInValue;
        this.toShowProfileMenu$ = this.getToShowProfileMenu;
        if (this.router.url.includes('/profile')) {
          this.isBlackBackground = true;
        } else {
          this.isBlackBackground = false;
        }
      });
  }

  get getToShowSignInValue() {
    if(this.auth.isLoggedIn) {
      this.toShowSignIn.next(false);
    } else {
      this.toShowSignIn.next(!this.router.url.includes('login'));
    }
    return this.toShowSignIn.asObservable();
  }

  get getToShowProfileMenu() {
    if(!this.auth.isLoggedIn) {
      this.toShowProfileMenu.next(false);
    }
    else {
      this.toShowProfileMenu.next(true);
    }
    return this.toShowProfileMenu.asObservable();
  }
  signOut() {
      this.auth.signOut(this.auth.fingerPrint);
      this.auth.deleteTokens();
      this.ngZone.run(() => this.router.navigate(['']));


  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
