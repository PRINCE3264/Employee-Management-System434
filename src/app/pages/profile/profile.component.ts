// import { MatIconModule } from '@angular/material/icon';
// import { MatInputModule } from '@angular/material/input';
// import { Component, inject } from '@angular/core';
// import { MatButtonModule } from '@angular/material/button';
// import { MatFormFieldModule } from '@angular/material/form-field';
// import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
// import { MatCardModule } from '@angular/material/card';
// import { AuthService } from '../../services/auth.service';

// @Component({
//   selector: 'app-profile',
//   imports: [MatFormFieldModule,
//     MatInputModule,
//     MatButtonModule,
//     ReactiveFormsModule,
//     MatCardModule,
//     MatIconModule],
//   templateUrl: './profile.component.html',
//   styleUrl: './profile.component.css'
// })
// export class ProfileComponent {
//   authService = inject(AuthService)
//   profileForm!: FormGroup
//   fb = inject(FormBuilder)
//   ngOnInit() {
//     this.profileForm = this.fb.group({
//       email: [],
//       profileImage: [],
//       phone: [],
//       name: [],
//       password: [],
//     })

//     this.authService.getProfile().subscribe(result => {
//       console.log(result)
//       this.profileForm.patchValue(result)
//     })
//   }
//   onUpdate() {
//     this.authService.updateProfile(this.profileForm.value).subscribe(result => {
//       alert("Profile Updated Successfull.....!✅✅")
//     })

//   }
// }



import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule
  ],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  profileForm!: FormGroup;

  private fb = inject(FormBuilder);
  private authService = inject(AuthService);

  ngOnInit(): void {
    this.profileForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      name: ['', [Validators.required, Validators.minLength(2)]],
      phone: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      //password: ['', Validators.minLength(6)], // ✅ remove required validator
      password: ['', [Validators.required, Validators.minLength(6)]],
      profileImage: ['']
    });

    this.authService.getProfile().subscribe({
      next: (result: any) => {
        if (result) {
          console.log(result)
          this.profileForm.patchValue(result);
          this.imageSrc = result.profileImage
        }
      },
      error: (err) => {
        console.error('❌ Failed to fetch profile:', err);
        alert('❌ Failed to load profile data.');
      }
    });
  }
  imageSrc!: string;

  fileUpload(event: Event) {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files[0]) {
      const file = target.files[0];
      const reader = new FileReader();
      reader.onload = () => {
        this.imageSrc = reader.result as string;
        this.profileForm.patchValue({
          profileImage: this.imageSrc
        })
        console.log(this.imageSrc);
      };
      reader.readAsDataURL(file);
    }
  }

  onUpdate(): void {
    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched(); // Highlight errors
      alert('⚠ Please fill all required fields correctly.');
      return;
    }

    this.authService.updateProfile(this.profileForm.value).subscribe({
      next: () => {
        alert('✅ Profile Updated Successfully!');
      },
      error: (err) => {
        console.error('Update Error:', err);
        alert('❌ Update Failed: ' + (err.error?.message || err.statusText));
      }
    });
  }

}
