import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { Tokens } from 'src/app/models/tokens';

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

  async ngOnInit() {
    if (!this.auth.fingerPrint) {
      await this.auth.createFingerprint();
    }
    this.auth.confirmEmail(this.userId, this.token)
      .subscribe(res => {
        this.auth.setTokens(res.body as Tokens);
      });
    this.router.navigate(['']);
  }
}
