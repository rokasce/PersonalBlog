import React, { useContext } from "react";
import ReactSwitch from "react-switch";
import { DarkThemeContext } from "../../../Hooks/DarkThemeHook";

export const DarkModeToggle = () => {
  const { turnOn, setTurnOn } = useContext(DarkThemeContext);

  const onChange = (checked: boolean, event: any) => {
    event.preventDefault();
    setTurnOn(checked);
  };

  return (
    <ReactSwitch
      checked={turnOn}
      onChange={(checked, event) => onChange(checked, event)}
    />
  );
};
