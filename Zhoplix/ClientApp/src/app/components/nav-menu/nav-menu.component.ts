import { Component, OnInit, NgZone, HostListener } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { Router, NavigationEnd, NavigationStart } from '@angular/router';
import { filter } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { ProfileService } from 'src/app/services/profile/profile.service';
import { NavbarAnimation } from 'src/app/animations/navbar-animation';

const routes: string[] = [];

@Component({
  selector: "app-nav-menu",
  templateUrl: "./nav-menu.component.html",
  styleUrls: ["./nav-menu.component.scss"],
  animations: [
    NavbarAnimation
  ]
})
export class NavMenuComponent implements OnInit {
  zhoplixLogoSrcLarge = "Logos/zhoplix_empty_167.png";
  zhoplixLogoSrcMedium = "Logos/zhoplix_empty_134.png";
  zhoplixLogoSrcSmall = "Logos/zhoplix_empty_108.png";
  isExpanded = false;
  toShowSignIn$ = new BehaviorSubject<boolean>(false);
  toShowProfileMenu$ = new BehaviorSubject<boolean>(false);
  isBackgroundBlack: boolean;
  avatar: string;
  animationState: string;

  constructor(private readonly router: Router,
              private readonly auth: AuthenticationService,
              private readonly profile: ProfileService,
              private readonly ngZone: NgZone) {
              }

  ngOnInit() {
    this.animationState = 'transparent';
    this.getBackgroundBlack();
    this.toShowProfileMenu();
    this.toShowSignIn();
    this.avatar = this.profile.getProfileImage();
    this.router.events
      .pipe(filter(event => event instanceof (NavigationEnd || NavigationStart)))
      .subscribe(() => {
        this.ngOnInit();
      });
    this.auth.avatarChange$.subscribe(() => {
      this.ngOnInit();
    })
  }

  @HostListener('window:scroll', ['$event']) // for window scroll events
  onScroll(event) {
    if(window.scrollY) {
      this.animationState = 'black';
    } else {
      this.animationState = 'transparent';
    }
  }

  toShowSignIn() {
    if(this.auth.isLoggedIn) {
      this.toShowSignIn$.next(false);
    } else {
      this.toShowSignIn$.next(!this.router.url.includes('/login'));
    }
  }

  toShowProfileMenu() {
    if(!this.auth.isLoggedIn) {
      this.toShowProfileMenu$.next(false);
    }
    else {
      this.toShowProfileMenu$.next(true);
    }
  }

  getBackgroundBlack() {
    if(this.router.url.includes('/profile')) {
      this.isBackgroundBlack = true;
    }
    else {
      this.isBackgroundBlack = false;
    }
  }

  signOut() {
      this.auth.signOut();
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
