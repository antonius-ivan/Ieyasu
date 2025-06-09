// src/global/brandPowerPoint.ts
import type { BrandVariants } from "../types";

/**
 * Microsoft PowerPoint core brand is roughly #D24726 (orange-red).
 * These 16 stops march from very dark (10) up to very light (160).
 */
export const brandPowerPoint: BrandVariants = {
    10: "#3A1A10",
    20: "#5E2418",
    30: "#7F2D1F",
    40: "#A03324",
    50: "#C13926",
    60: "#D24726",  // core PPT orange-red
    70: "#DC5A3D",
    80: "#E46F54",
    90: "#EF8A73",
    100: "#F3A38D",
    110: "#F8B6A3",
    120: "#FCD6CA",
    130: "#FDE3D9",
    140: "#FEECE3",
    150: "#FEEFE8",
    160: "#FFFAF8",
};

// src/themes/powerPointLightTheme.ts
//import { createLightTheme } from "../utils/createLightTheme";
//import { brandPowerPoint } from "../global/brandPowerPoint";
//import type { Theme } from "../types";

//export const powerPointLightTheme: Theme = createLightTheme(brandPowerPoint);
