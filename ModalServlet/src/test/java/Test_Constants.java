import com.powerdms.Constants;

class Test_Constants {
    private static final String TITLE = "Title - Browser Name";
    private static final String PATH = "p:\\ath\\to.file";
    private static final String TIMEOUT = "1";
    private static final String USERNAME = "user";
    private static final String PASSWORD = "pass";
    static final String HANDLER_OUT = "Handler test output";
    // upload -o [owner] -p [path] -t [timeout]
    static final String[] CORRECT_ARGS_UPLOAD = {
            Constants.PARAM_UPLOAD,
            prependDash(Constants.PARAM_UPLOAD_OWNER), TITLE,
            prependDash(Constants.PARAM_UPLOAD_PATH), PATH,
            prependDash(Constants.PARAM_TIMEOUT), TIMEOUT};
    // auth -u [username] -p [password] -t [timeout]
    static final String[] CORRECT_ARGS_AUTH = {
            Constants.PARAM_AUTH,
            prependDash(Constants.PARAM_AUTH_USERNAME), USERNAME,
            prependDash(Constants.PARAM_AUTH_PASSWORD), PASSWORD,
            prependDash(Constants.PARAM_TIMEOUT), TIMEOUT};
    // cleanup -t [timeout]
    static final String[] CORRECT_ARGS_CLEANUP = {
            Constants.PARAM_CLEANUP,
            prependDash(Constants.PARAM_TIMEOUT), TIMEOUT};

    static final String CORRECT_QUERY_UPLOAD = Constants.PARAM_UPLOAD +
            formatIntoQuery(Constants.PARAM_UPLOAD_OWNER, TITLE) +
            formatIntoQuery(Constants.PARAM_UPLOAD_PATH, PATH) +
            formatIntoQuery(Constants.PARAM_TIMEOUT, TIMEOUT);

    static final String CORRECT_QUERY_CLEANUP = Constants.PARAM_CLEANUP +
            formatIntoQuery(Constants.PARAM_TIMEOUT, TIMEOUT);

    static final String CORRECT_QUERY_AUTH = Constants.PARAM_AUTH +
            formatIntoQuery(Constants.PARAM_AUTH_USERNAME, USERNAME) +
            formatIntoQuery(Constants.PARAM_AUTH_PASSWORD, PASSWORD) +
            formatIntoQuery(Constants.PARAM_TIMEOUT,  TIMEOUT);

    private static String prependDash(String str) {
        return format('-', '\0', str);
    }

    private static String formatIntoQuery(String... params) {
        return format('&', ' ', params);
    }

    private static String format(Character prependChar, Character separateChar, String... args) {
        String str = String.valueOf(prependChar);
        for (String arg : args)
            str += arg + separateChar;
        return str.trim();
    }
}
