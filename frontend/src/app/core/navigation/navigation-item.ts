export interface NavigationItem {
    label: string;
    route: string;
    icon?: string;
    children?: NavigationItem[];
}

