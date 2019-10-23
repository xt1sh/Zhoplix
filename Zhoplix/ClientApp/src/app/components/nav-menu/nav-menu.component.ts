import { Component, OnInit } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';

const routes: string[] = [];

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  zhoplixLogoSrcLarge = 'Logos/zhoplix_empty_167.png';
  zhoplixLogoSrcMedium = 'Logos/zhoplix_empty_134.png';
  zhoplixLogoSrcSmall = 'Logos/zhoplix_empty_108.png';
  isExpanded = false;
  toShow$: Observable<boolean>;
  toShow: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor(private readonly router: Router) {
    router.events.subscribe(() => {
      this.toShow$ = this.getToShowValue;
    })
  }

  ngOnInit() {
    this.toShow$ = this.getToShowValue;
  }

  get getToShowValue() {
    this.toShow.next(!routes.includes(this.router.url.slice(1)));
    return this.toShow.asObservable();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
