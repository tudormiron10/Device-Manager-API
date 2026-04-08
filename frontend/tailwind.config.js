/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      colors: {
        'primary': '#172A3A',
        'primary-hover': '#254521',
        'accent': '#5e0a0a',
        'light': '#ffaf87',
        'secondary': '#772F1A',
        'dark': '#8D909B'
      }
    },
  },
  plugins: [],
}

