import { Button } from '@mantine/core';

interface TopBarButtonProps {
  text: string;
  icon: React.ReactNode;
  color: string;
  link?: string;
  isMobile: boolean;
  onClick?: (event: { preventDefault: () => void }) => void;
}
const TopBarButton = ({
  text,
  icon,
  color,
  link,
  isMobile,
  onClick,
}: TopBarButtonProps) => {
  return isMobile ? (
    <Button
      component="a"
      target="_blank"
      rel="noopener noreferrer"
      onClick={onClick}
      href={link}
      color={color}>
      {icon}
    </Button>
  ) : (
    <Button
      onClick={onClick}
      component="a"
      target="_blank"
      rel="noopener noreferrer"
      href={link}
      color={color}
      leftIcon={icon}>
      {text}
    </Button>
  );
};

export default TopBarButton;
