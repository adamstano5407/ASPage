import { Component } from "@angular/core";
import { navigationConfig } from "../../core/navigation/navigation-config";
import { RouterLink, RouterLinkActive } from "@angular/router";

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.html',
    styleUrls: ['./navbar.scss'],
    imports: [RouterLink, RouterLinkActive]
})

export class Navbar {
    protected readonly items = navigationConfig;

}