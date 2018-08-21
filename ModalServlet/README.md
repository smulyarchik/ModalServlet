# ModalServlet

##
### Handles modal dialogs in [Selenium GRID](http://www.seleniumhq.org/projects/grid/) environment by making calls to [ModalHandler](../ModalHandler) console application.

#### Steps to assemble the project:

 1. Build [ModalHandler](../ModalHandler/ModalHandler.sln) solution. There is a post-build step that copies it's output to the servlet's `src\resources` folder.
 2. Import maven project pointing your IDE to **pom.xml**.
 3. Mark `src\java` as *Sources*.
 4. Mark `src\resources` as *Resources*.
 5. Mark `src\test\java` as *Tests*.

#### JAR file should contain:

 * Extracted **commons-io-1.3.2.jar**.
 * Extracted **javax.json-1.0.4.jar**.
 * Extracted **javax.json-api-1.1.jar**.
 * Extracted **javax.servlet-api-3.1.0.jar**.
 * **ModalServlet** compile output.

#### Usage: 
Start the server using the the following commands.

***Hub***: `java -cp *;. org.openqa.grid.selenium.GridLauncher -role hub -servlets <servlet name>`

***Node***: `java -cp *;. org.openqa.grid.selenium.GridLauncher -role node -hub <hub address> -servlets <servlet name>`

**Note**: Windows environment uses `;` for classpath separator. For Linux use `:`.

### Related links:

 * **[Customizing GRID](http://docs.seleniumhq.org/docs/07_selenium_grid.jsp#customizing-the-grid)** section for further reading.
