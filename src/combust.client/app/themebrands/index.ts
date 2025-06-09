// src//themebrands/index.ts
import type { BrandVariants } from "@fluentui/react-components";
import { createLightTheme, createDarkTheme, Theme } from "@fluentui/react-components";

import { brandExcel } from "./brandExcel";
import { brandPowerPoint } from "./brandPowerPoint";
import { brandPowerBI } from "./brandPowerBI";
import { brandOneNote } from "./brandOneNote";
import { brandWeb } from "./brandWeb";

const raw: Record<string, BrandVariants> = {
    web: brandWeb,
    excel: brandExcel,
    powerPoint: brandPowerPoint,
    powerBI: brandPowerBI,
    oneNote: brandOneNote,
};

// map each raw brand -> light & dark theme
export const lightThemes: Record<keyof typeof raw, Theme> = Object.fromEntries(
    Object.entries(raw).map(([key, tokens]) => [key, createLightTheme(tokens)])
) as any;

export const darkThemes: Record<keyof typeof raw, Theme> = Object.fromEntries(
    Object.entries(raw).map(([key, tokens]) => [key, createDarkTheme(tokens)])
) as any;

export const unifiedThemes = Object.entries(raw).flatMap(([key, tokens]) => [
    { brand: key, mode: "light", theme: createLightTheme(tokens) },
    { brand: key, mode: "dark", theme: createDarkTheme(tokens) },
]);

export type BrandKey = keyof typeof raw;
export type Mode = "light" | "dark";
