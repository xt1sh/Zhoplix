import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';

@Component({
  selector: 'app-confirmEmail',
  templateUrl: './confirmEmail.component.html',
  styleUrls: ['./confirmEmail.component.scss']
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
  
    this.auth.confirmEmail(this.userId, this.token)
    .subscribe(res => {
      this.auth.setToken(res);
    });
    this.router.navigate(['']);
  }

}
