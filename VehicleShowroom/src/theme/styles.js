import { mode } from '@chakra-ui/theme-tools';
export const globalStyles = {
  colors: {
    brand: {
      100: '#F7F7F7', // very light gray
      200: '#E0E0E0', // light gray
      300: '#CFCFCF', // soft gray
      400: '#A0A0A0', // medium gray
      500: '#707070', // main gray tone
      600: '#4A4A4A', // dark gray
      700: '#2E2E2E', // darker gray
      800: '#1A1A1A', // near black
      900: '#0A0A0A', // pure black tone
    },

    brandScheme: {
      100: '#F7F7F7',
      200: '#E0E0E0',
      300: '#CFCFCF',
      400: '#A0A0A0',
      500: '#707070',
      600: '#4A4A4A',
      700: '#2E2E2E',
      800: '#1A1A1A',
      900: '#0A0A0A',
    },

    brandTabs: {
      100: '#F7F7F7',
      200: '#E0E0E0',
      300: '#CFCFCF',
      400: '#A0A0A0',
      500: '#707070',
      600: '#4A4A4A',
      700: '#2E2E2E',
      800: '#1A1A1A',
      900: '#0A0A0A',
    },

    secondaryGray: {
      100: '#E0E5F2',
      200: '#E1E9F8',
      300: '#F4F7FE',
      400: '#E9EDF7',
      500: '#8F9BBA',
      600: '#A3AED0',
      700: '#707EAE',
      800: '#707EAE',
      900: '#1B2559',
    },
    red: {
      100: '#FEEFEE',
      500: '#EE5D50',
      600: '#E31A1A',
    },
    blue: {
      50: '#EFF4FB',
      500: '#3965FF',
    },
    orange: {
      100: '#FFF6DA',
      500: '#FFB547',
    },
    green: {
      100: '#E6FAF5',
      500: '#01B574',
    },
    navy: {
      50: '#d0dcfb',
      100: '#aac0fe',
      200: '#a3b9f8',
      300: '#728fea',
      400: '#3652ba',
      500: '#1b3bbb',
      600: '#24388a',
      700: '#1B254B',
      800: '#111c44',
      900: '#0b1437',
    },
    gray: {
      100: '#FAFCFE',
    },
  },
  styles: {
    global: (props) => ({
      body: {
        overflowX: 'hidden',
        bg: mode('secondaryGray.300', 'navy.900')(props),
        fontFamily: 'DM Sans',
        letterSpacing: '-0.5px',
      },
      input: {
        color: 'gray.700',
      },
      html: {
        fontFamily: 'DM Sans',
      },
    }),
  },
};
