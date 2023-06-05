const statusMessages = [
    {
        code: 400,
        message: 'Something weird is going on right now!',
        bg: 'bg-danger'
    },
    {
        code: 409,
        message: 'User with this email address exists already, please try to identify yourself with the email you just attempted to register.',
        bg: 'bg-warning'
    },
    {
        code: 500,
        message: 'Wehhhhhhhhhheeeeeeeeeeeeeeeeeeeee!',
        bg: 'bg-danger'
    },
    {
        code: 201,
        message: 'Successfully registered',
        bg: 'bg-success'
    }
];

export const getStatusMessage = (code) => {
    return statusMessages.find(messageObject => messageObject.code === code)
}