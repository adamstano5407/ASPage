
import { Type } from '@angular/core';
import type { LucideIcon } from '@lucide/angular';

export interface NavigationItem {
    label: string;
    route: string;
    icon?: LucideIcon | null;
    children?: NavigationItem[];
    disabled?: boolean;
    visible?: boolean;
    external?: boolean;
}

