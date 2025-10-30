import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatIconModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  authService = inject(AuthService);
  fb = inject(FormBuilder);
  router = inject(Router);

  loginForm!: FormGroup;

  ngOnInit() {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });


    if (this.authService.isLoggedIn) {
      const role = this.authService.userRole;
      console.log('Auto Login Role:', role);


      if (role === 'Admin') {
        this.router.navigate(['/']);
      } else if (role === 'Employee') {
        this.router.navigate(['/employee-dashboard']);
      }
    }
  }
  OnLogin() {
    if (this.loginForm.invalid) return;
    const { email, password } = this.loginForm.value;
    this.authService.Login(email, password).subscribe({
      next: (result) => {
        console.log('Login result:', result);
        if (result?.token && result?.role) {
          this.authService.saveToken(result);
          const role = result.role;
          if (role === 'Admin') {
            this.router.navigate(['/']);
          } else if (role === 'Employee') {
            this.router.navigate(['/employee-dashboard']);
          } else {
            console.warn('Unknown role:', role);
            this.router.navigate(['/']);
          }
        } else {
          console.error('Login response missing token or role');
        }
      },
      error: (err) => {
        console.error('Login failed:', err);
      }
    });
  }
}


