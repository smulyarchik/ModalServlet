import com.powerdms.Constants;
import com.powerdms.Messages;
import com.powerdms.ModalHandler;
import com.powerdms.Util;
import org.hamcrest.CoreMatchers;
import org.hamcrest.core.IsEqual;
import org.junit.After;
import org.junit.Assert;
import org.junit.Ignore;
import org.junit.Test;
import org.junit.experimental.ParallelComputer;
import org.junit.runner.JUnitCore;
import org.junit.runner.Result;

import java.io.File;
import java.text.SimpleDateFormat;
import java.util.Date;

public class ModalHandlerTest {
    private final ModalHandler handler = new ModalHandler();

    @After
    public void TestTeardown() {
        System.out.println(handler.getOut());
    }

    @Test
    public void load() {
        Assert.assertTrue(handler.load());
        String out = handler.getOut();
        Assert.assertThat(out, CoreMatchers.containsString(Constants.MODAL_HANDLER_LIB));
        Assert.assertThat(out, CoreMatchers.containsString(Constants.MODAL_HANDLER_CONSOLE));
    }

    @Test
    public void exec_out() throws Exception {
        handler.load();
        String loadOut = handler.getOut();
        // Passing no arguments to check basic communication.
        int exitCode = handler.exec(new String[0]);
        Assert.assertEquals(160, exitCode);
        // Copy-pasted from ModalHandler console app. Subject for a change, should the application change.
        String argumentMismatchMsg = "ModalHandler 1.0.0.0\r\n" +
                "Copyright c  2017\r\n" +
                "\r\n" +
                "  upload     Handle native file upload dialog for a specified window.\r\n" +
                "\r\n" +
                "  cleanup    Close all open modal dialogs.\r\n" +
                "\r\n" +
                "  auth       Handle IE11 'Windows Security' dialog.";
        Assert.assertThat(handler.getOut().replace(loadOut, "").trim(), IsEqual.equalTo(argumentMismatchMsg));
    }

    @Test
    public void exec_upload() throws Exception {
        // Should time out on modal dialog detection, therefore passing arguments parsing.
        exec_correctArgs(Test_Constants.CORRECT_ARGS_UPLOAD, 1);
    }

    @Test
    public void exec_cleanUp() throws Exception {
        // Should succeed.
        exec_correctArgs(Test_Constants.CORRECT_ARGS_CLEANUP, 0);
    }

    @Test
    public void exec_auth() throws Exception {
        // Should time out on modal dialog detection, therefore passing arguments parsing.
        exec_correctArgs(Test_Constants.CORRECT_ARGS_AUTH, 1);
    }

    private void exec_correctArgs(String[] args, int expectedExitCode) throws Exception {
        boolean isSuccess = handler.load();
        int exitCode = handler.exec(args);
        isSuccess &= expectedExitCode == exitCode;
        Assert.assertTrue(isSuccess);
    }
}
