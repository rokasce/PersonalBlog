import { createContext, useEffect, useState } from "react";

interface Themes {
  dark: string;
  light: string;
}

interface DarkThemeContextProps {
  children?: JSX.Element;
}

const themes: Themes = {
  dark: "dark",
  light: "light",
};

const DarkThemeContext = createContext({
  turnOn: true,
  setTurnOn: (turnedOn: boolean) => {},
  theme: themes.dark,
  setTheme: (theme: keyof Themes) => {},
});

const DarkThemeProvider = ({ children }: DarkThemeContextProps) => {
  const [turnOn, setTurnOn] = useState<boolean>(false);
  const [theme, setTheme] = useState(themes.dark);

  useEffect(() => {
    const color = turnOn ? themes.dark : themes.light;

    document
      .getElementsByTagName("body")[0]
      .classList.add(turnOn ? themes.dark : themes.light);
    document
      .getElementsByTagName("body")[0]
      .classList.remove(turnOn ? themes.light : themes.dark);

    setTheme(color);
  }, [turnOn]);

  return (
    <DarkThemeContext.Provider value={{ turnOn, setTurnOn, theme, setTheme }}>
      {children}
    </DarkThemeContext.Provider>
  );
};

export { themes, DarkThemeContext, DarkThemeProvider };
