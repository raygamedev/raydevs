import {
  createStyles,
  Avatar,
  Text,
  Group,
  CopyButton,
  Tooltip,
  ActionIcon,
} from '@mantine/core';
import {
  IconPhoneCall,
  IconAt,
  IconCheck,
  IconCopy,
} from '@tabler/icons-react';
import ProfilePicture from '../../art/profiePicture.png';
const useStyles = createStyles((theme) => ({
  root: {
    width: '80%',
  },
  icon: {
    color:
      theme.colorScheme === 'dark'
        ? theme.colors.dark[3]
        : theme.colors.gray[5],
  },

  name: {
    fontFamily: `${theme.fontFamily}`,
  },
}));

interface ProfileType {
  avatar: string;
  name: string;
  title: string;
  phone: string;
  email: string;
}
const ProfileData: ProfileType = {
  avatar: ProfilePicture,
  name: 'Dan Raymond',
  title: 'Fullstack Software Developer',
  phone: '+972-526865438',
  email: 'dan@raydevs.com',
};
// TODO: Make phone and email copyable
export function Profile() {
  const { avatar, name, title, phone, email } = ProfileData;
  const { classes } = useStyles();
  return (
    <Group
      style={{
        display: 'flex',
        justifyContent: 'center',
      }}>
      <Avatar src={avatar} size={200} radius="md" />
      <div>
        <Text fz="md" tt="uppercase" fw={700} c="dimmed">
          {title}
        </Text>

        <Text fz="xl" fw={500} className={classes.name}>
          {name}
        </Text>

        <Group noWrap spacing={10} mt={10}>
          <IconAt stroke={1.5} size="1rem" className={classes.icon} />
          <Text fz="md" c="dimmed">
            {email}
          </Text>
          <CopyButton value={email} timeout={2000}>
            {({ copied, copy }) => (
              <Tooltip
                label={copied ? 'Copied' : 'Copy'}
                withArrow
                position="right">
                <ActionIcon color={copied ? 'violet' : 'gray'} onClick={copy}>
                  {copied ? (
                    <IconCheck size="1rem" />
                  ) : (
                    <IconCopy size="1rem" />
                  )}
                </ActionIcon>
              </Tooltip>
            )}
          </CopyButton>
        </Group>

        <Group noWrap spacing={10} mt={5}>
          <IconPhoneCall stroke={1.5} size="1rem" className={classes.icon} />
          <Text fz="md" c="dimmed">
            {phone}
          </Text>
          <CopyButton value={phone} timeout={2000}>
            {({ copied, copy }) => (
              <Tooltip
                label={copied ? 'Copied' : 'Copy'}
                withArrow
                position="right">
                <ActionIcon color={copied ? 'violet' : 'gray'} onClick={copy}>
                  {copied ? (
                    <IconCheck size="1rem" />
                  ) : (
                    <IconCopy size="1rem" />
                  )}
                </ActionIcon>
              </Tooltip>
            )}
          </CopyButton>
        </Group>
      </div>
    </Group>
  );
}
