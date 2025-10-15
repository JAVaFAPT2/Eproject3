import { mode } from '@chakra-ui/theme-tools';
export const globalStyles = {
  colors: {
    brand: {
      100: '#F7F7F7', // trắng nhạt
      200: '#E0E0E0', // xám sáng
      300: '#CFCFCF', // xám nhẹ
      400: '#AFAFAF', // trung tính
      500: '#7A7A7A', // xám chuẩn brand
      600: '#4D4D4D', // xám đậm
      700: '#2C2C2C', // gần đen
      800: '#1A1A1A', // rất đen
      900: '#0D0D0D', // đen tuyệt đối
    },
    brandScheme: {
      100: '#FFFFFF',
      200: '#F2F2F2',
      300: '#E5E5E5',
      400: '#CCCCCC',
      500: '#999999',
      600: '#666666',
      700: '#333333',
      800: '#1A1A1A',
      900: '#000000',
    },
    brandTabs: {
      100: '#F9F9F9',
      200: '#EAEAEA',
      300: '#DADADA',
      400: '#B5B5B5',
      500: '#888888',
      600: '#5C5C5C',
      700: '#2E2E2E',
      800: '#1A1A1A',
      900: '#000000',
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
