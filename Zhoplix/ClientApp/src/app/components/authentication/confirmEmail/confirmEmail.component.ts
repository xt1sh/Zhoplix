import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';

@Component({
  selector: 'app-confirmEmail',
})
export class ConfirmEmailComponent implements OnInit {

  userId: string;
  token: string;
  
  constructor(private route: ActivatedRoute,
              private auth: AuthenticationService,
              private router: Router) {

  this.route.queryParams.subscribe(params => {
                  this.userId = params['userId'];
                  this.token = params['token'];
              });
}

  ngOnInit() {
    
    const confirmEmailObservable = this.auth.createFingerprint().subscribe(value => {
    this.auth.confirmEmail(this.userId, this.token, value)
      .subscribe(res => {
        this.auth.setToken(res);
      });
    this.router.navigate(['']);
    });
  }

}
