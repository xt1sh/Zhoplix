import { Component, OnInit } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';

const routes: string[] = [];

@Component({
  selector: "app-nav-menu",
  templateUrl: "./nav-menu.component.html",
  styleUrls: ["./nav-menu.component.css"]
})
export class NavMenuComponent implements OnInit {
  zhoplixLogoSrcLarge = "Logos/zhoplix_empty_167.png";
  zhoplixLogoSrcMedium = "Logos/zhoplix_empty_134.png";
  zhoplixLogoSrcSmall = "Logos/zhoplix_empty_108.png";
  isExpanded = false;
  toShow$: Observable<boolean>;
  toShow: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  toShowSignIn$: Observable<boolean>;
  toShowSignIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor(private readonly router: Router,
              private readonly auth: AuthenticationService) {}

  ngOnInit() {
    this.toShow$ = this.getToShowValue;
    this.toShowSignIn$ = this.getToShowSignInValue;
    const event = this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.toShow$ = this.getToShowValue;
        this.toShowSignIn$ = this.getToShowSignInValue;
      });
  }

  get getToShowValue() {
    this.toShow.next(!routes.includes(this.router.url.slice(1)));
    return this.toShow.asObservable();
  }

  get getToShowSignInValue() {
    if(this.auth.isLoggedIn) {
      this.toShowSignIn.next(false);
    } else {
      this.toShowSignIn.next(!this.router.url.includes("login"));
    }
    return this.toShowSignIn.asObservable();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
